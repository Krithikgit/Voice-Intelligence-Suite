import os
import queue
import re
import sys
import json

from google.cloud import speech
from google.cloud import translate_v2 as translate  # Import Google Translate API
import pyaudio

# Set up Google Cloud credentials
os.environ["GOOGLE_APPLICATION_CREDENTIALS"] = r"C:\Users\krith\source\repos\miniproj\miniproj\serv_acc.json"

# Audio parameters
RATE = 16000
CHUNK = int(RATE / 10)  # 100ms


class MicrophoneStream:
    """Opens a recording stream as a generator yielding the audio chunks."""

    def __init__(self, rate=RATE, chunk=CHUNK):
        self._rate = rate
        self._chunk = chunk
        self._buff = queue.Queue()
        self.closed = True

    def __enter__(self):
        self._audio_interface = pyaudio.PyAudio()
        self._audio_stream = self._audio_interface.open(
            format=pyaudio.paInt16,
            channels=1,
            rate=self._rate,
            input=True,
            frames_per_buffer=self._chunk,
            stream_callback=self._fill_buffer,
        )
        self.closed = False
        return self

    def __exit__(self, type, value, traceback):
        self._audio_stream.stop_stream()
        self._audio_stream.close()
        self.closed = True
        self._buff.put(None)
        self._audio_interface.terminate()

    def _fill_buffer(self, in_data, frame_count, time_info, status_flags):
        self._buff.put(in_data)
        return None, pyaudio.paContinue

    def generator(self):
        while not self.closed:
            chunk = self._buff.get()
            if chunk is None:
                return
            data = [chunk]

            while True:
                try:
                    chunk = self._buff.get(block=False)
                    if chunk is None:
                        return
                    data.append(chunk)
                except queue.Empty:
                    break

            yield b"".join(data)


def translate_text(text, target_language="es"):
    """Translates text into the target language using Google Translate API."""
    translate_client = translate.Client()
    result = translate_client.translate(text, target_language=target_language)
    return result["translatedText"]


def listen_print_loop(responses):
    """Processes responses and returns transcription and translation in JSON format."""
    final_transcription = ""

    for response in responses:
        if not response.results:
            continue

        result = response.results[0]
        if not result.alternatives:
            continue

        transcript = result.alternatives[0].transcript

        if result.is_final:
            final_transcription += transcript + " "

            if re.search(r"\b(exit|quit)\b", transcript, re.I):
                break

    # Strip any extra spaces
    final_transcription = final_transcription.strip()

    try:
        # Translate the final transcription to the desired language (e.g., Spanish)
        translated_text = translate_text(final_transcription, target_language="es")

        # Create a JSON object with transcription and translation
        result_json = json.dumps({
            "status": "success",
            "transcription": final_transcription,
            "translation": translated_text
        })
    except Exception as e:
        # Handle any errors and return an error message in JSON format
        result_json = json.dumps({"status": "error", "message": str(e)})

    # Print only the final JSON (important!)
    print(result_json)


def main():
    language_code = "en-US"
    client = speech.SpeechClient()

    config = speech.RecognitionConfig(
        encoding=speech.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=RATE,
        language_code=language_code,
    )

    streaming_config = speech.StreamingRecognitionConfig(
        config=config, interim_results=True
    )

    with MicrophoneStream(RATE, CHUNK) as stream:
        audio_generator = stream.generator()
        requests = (speech.StreamingRecognizeRequest(audio_content=content) for content in audio_generator)

        responses = client.streaming_recognize(streaming_config, requests)

        listen_print_loop(responses)


if __name__ == "__main__":
    main()


