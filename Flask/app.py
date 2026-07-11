from flask import Flask
from gemini_data import GeminiData



app = Flask(__name__)

@app.route("/", methods = ["GET"])
def test():
    contents = None
    with open("MU_sound_check") as file:
        contents = file.read()
    
    return contents