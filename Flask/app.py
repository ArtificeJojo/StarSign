from flask import Flask
from flask import request
from google import genai
import os
import elevenlabs
from google.genai import interactions
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


"""Gets the zodiac signs which is given by the player
at the beggining of the game"""
@app.route("/zodiac", methods=["POST"])
def get_zodiac_sign():
    aiData.star_key = str(request.form["zodiac_sign"])
    
    return "THANK YOU!!!"

"""Requests from the game to get a response from Barnum to comment on what the
player has done"""
@app.route("/barnum", methods = ["POST"])
def send_barnum_response():
    try:
        aiData.star_key = "Cancer"
        prompt = str(request.form["prompt"])
        return aiData.sendBarnumPrompt(prompt)
    except KeyError:
        print("DIDN'T SEND PROMPT")
        return "Error"

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
    # The request didn't have the prompt response
    try:
        response= client.interactions.create(
            model=aiData.model,
            input=request.form["prompt"],
            system_instruction=aiData.BARNUM_PROMPT,
            stream=False
        )
        print("Prompt Finished")
    except KeyError:
        print("GOT KEY ERROR LOL")
        return "Error"
    
    if (type(response) != interactions.Interaction):
        print(f"Invalid response, or somehow a stream: {type(response)}")
        return "Error"
    
    if response is None:
        print("IT RETURNED NOTHING BUDDY LOL")
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