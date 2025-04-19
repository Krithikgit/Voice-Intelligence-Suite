import whisper
import numpy as np
import torch
import librosa
from resemblyzer import VoiceEncoder, preprocess_wav
from sklearn.cluster import DBSCAN
from itertools import groupby
import sys
import json

# === Load audio from command line ===
if len(sys.argv) < 2:
    print("Usage: python pydia.py <audio_path>", file=sys.stderr)
    sys.exit(1)

audio_path = sys.argv[1]

print("Transcribing with Whisper...", file=sys.stderr)

# Transcribe with Whisper
model = whisper.load_model("base")
result = model.transcribe(audio_path, language="en", word_timestamps=True)
segments = result["segments"]
print("Transcription completed", file=sys.stderr)

# === Speaker Diarization ===
print("Loading voice encoder...", file=sys.stderr)

import contextlib
import io
# Suppress stdout temporarily while loading the encoder
with contextlib.redirect_stdout(io.StringIO()):
    encoder = VoiceEncoder()

print("Loaded the voice encoder model on", torch.device("cpu").type, file=sys.stderr)

print("Generating embeddings...", file=sys.stderr)
wav, sr = librosa.load(audio_path, sr=16000)
segment_embeddings = []

for seg in segments:
    start = int(seg["start"] * sr)
    end = int(seg["end"] * sr)
    clip = wav[start:end]
    preprocessed = preprocess_wav(clip, source_sr=sr)
    embedding = encoder.embed_utterance(preprocessed)
    segment_embeddings.append(embedding)

embeddings = np.vstack(segment_embeddings)

# === Clustering ===
print("Clustering embeddings...", file=sys.stderr)
clustering = DBSCAN(metric="cosine", eps=0.25, min_samples=1).fit(embeddings)
speaker_labels = clustering.labels_

# === Grouped Diarization Result ===
print("\n=== Diarization Result ===\n", file=sys.stderr)

output = []

for speaker, group in groupby(zip(speaker_labels, segments), key=lambda x: x[0]):
    group = list(group)
    texts = [seg["text"].strip() for _, seg in group]
    start = group[0][1]["start"]
    end = group[-1][1]["end"]
    print(f"Speaker {speaker}: {' '.join(texts)} [{start:.2f}s - {end:.2f}s]", file=sys.stderr)
    output.append({
        "SpeakerId": int(speaker),
        "Start": round(start, 2),
        "End": round(end, 2),
        "Text": " ".join(texts)
    })

# ✅ Return only JSON to C#
print(json.dumps(output), flush=True)