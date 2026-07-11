from flask import Flask
from google import genai
import os
from dotenv import load_dotenv
from werkzeug.middleware.proxy_fix import ProxyFix
from gemini_data import GeminiData



app = Flask(__name__)
app.wsgi_app = ProxyFix(app.wsgi_app)

@app.route("/", methods = ["GET"])
def test():
    contents = None
    with open("MU_sound_check_ignore.wav", "rb+") as file:
        contents = file.read()
    
    return contents

@app.route("/ai", methods= ["POST"])
def audio_test():
    
    load_dotenv()
    client = genai.Client(api_key=os.getenv("GEMINI_API"))
    return ""