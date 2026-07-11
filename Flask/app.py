from flask import Flask
from flask import request
from google import genai
import os
import elevenlabs
from dotenv import load_dotenv
from werkzeug.middleware.proxy_fix import ProxyFix
from gemini_data import GeminiData



app = Flask(__name__)
app.wsgi_app = ProxyFix(app.wsgi_app)

aiData = GeminiData()

@app.route("/", methods = ["GET"])
def test():
    contents = None
    with open("MU_sound_check_ignore.wav", "rb+") as file:
        contents = file.read()
    
    return contents


"""Testing the app capacities to send audio files
made by Eleven Labs based on prompt responses from Gemini"""
@app.route("/ai", methods= ["POST"])
def audio_test():
    
    # contents = None
    # with open("MU_sound_check_ignore.wav", "rb+") as file:
    #     contents = file.read()
    
    # return contents
    
    load_dotenv()
    client = genai.Client(api_key=os.getenv("GEMINI_API"))
    elevenClient = elevenlabs.ElevenLabs(api_key=os.getenv("ELEVEN_API"))
    print(request.form.keys())
    
    print("Sending prompt")
    try:
        response = client.interactions.create(
            model="gemini-3.1-flash-lite",
            input=request.form["prompt"],
            system_instruction=aiData.BARNUM_PROMPT
        )
        print("Prompt Finished")
    except KeyError:
        print("GOT KEY ERROR LOL")
        return "Error"
    
    if response.output_text is None:
        print("IT RETURNED NOTHING BUDDY LOL")
        return "Error"
    
    print("Creating audio")
    elevenResponse = elevenClient.text_to_speech.convert(
        voice_id="l1zE9xgNpUTaQCZzpNJa",
        text = response.output_text,
        output_format="mp3_44100_128",
        model_id="eleven_multilingual_v2"
    )
    
    print("Audio complete")
    
    return elevenResponse