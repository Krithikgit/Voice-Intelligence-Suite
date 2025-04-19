import sys
from transformers import T5Tokenizer, T5ForConditionalGeneration

# Receive transcription text from the command line
transcription_text = sys.argv[1] if len(sys.argv) > 1 else ""

tokenizer = T5Tokenizer.from_pretrained("google/flan-t5-small")
model = T5ForConditionalGeneration.from_pretrained("google/flan-t5-small")

input_text = "summarize: " + transcription_text
input_ids = tokenizer(input_text, return_tensors="pt").input_ids

outputs = model.generate(input_ids)
summary = tokenizer.decode(outputs[0], skip_special_tokens=True)  # Added skip_special_tokens=True to clean up the output

print(summary)
