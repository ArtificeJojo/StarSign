"""It must at least succeed at these imports
for the requirements.txt trick to work"""
import subprocess
import sys

try:
    from flask import Flask
    from flask import request
    import random
    from werkzeug.middleware.proxy_fix import ProxyFix
    from gemini_data import GeminiData
except ImportError:
    # Auto-installs all requirements
    print("Missing requirements... Auto installing them")
    subprocess.check_call([sys.executable, "-m", "pip", "install", "-r", "requirements.txt"])
    
    # Try again. If it fails here, it should abort
    from flask import Flask
    from flask import request
    import random
    from werkzeug.middleware.proxy_fix import ProxyFix
    from gemini_data import GeminiData


# Our list of all the connections
allConnections: dict[int, GeminiData] = {
    
}

# All the ids. A set to enforce all of them being unique
idList = set()


app = Flask(__name__)
app.wsgi_app = ProxyFix(app.wsgi_app)



"""Gets the zodiac signs which is given by the player
at the beggining of the game"""
@app.route("/zodiac", methods=["POST"])
def get_zodiac_sign():
    
    newID = random.randint(0, 20000000)
    while newID in idList:
        newID = random.randint(0, 20000000)
    
    allConnections[newID] = GeminiData()
    
    newConnection = allConnections[newID] 
    try:
        if request.form["zodiac"] not in list(newConnection.star_signs.keys()):
            print("Did not receive the proper zodiac sign")
            return "NOT A PROPER ZODIAC SIGN"
    except KeyError:
        print("DID NOT SEND ZODIAC SIGN")
        return "Error"
    
    allConnections[newID].star_key = request.form["zodiac"]
    
    idList.add(newID)
    
    print(f"User with ID {newID} added")
    
    return str(newID)

"""Requests the server to disconnect the id from the list
of players"""
@app.route("/disconnect/<int:id>", methods=["GET"])
def disconnect_app(id):
    
    # Remove entry from our connection list
    allConnections.pop(id)
    
    idList.remove(id)
    
    return "DISCONNECTED"

"""Requests from the game to get a response from Barnum to comment on what the
player has done"""
@app.route("/barnum/<int:id>", methods = ["POST"])
def send_barnum_response(id):
    if id not in idList:
        print("Invalid ID")
        return "Error"
    
    aiData = allConnections[id]
    
    try:
        prompt = str(request.form["prompt"])
        return aiData.sendBarnumPrompt(prompt)
    except KeyError:
        print("DIDN'T SEND PROMPT")
        return "Error"