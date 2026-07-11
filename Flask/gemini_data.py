import tiktoken
from google import genai
from flask import Request
import elevenlabs
from google.genai import interactions
import os
from dotenv import load_dotenv

class GeminiData:
    
    star_key: str | None = None
    
    # Data on the actual model
    model = "gemini-3.1-flash-lite"
    token_limit = 1048576
    lastInteraction: interactions.Interaction | None = None
    
    star_signs: dict = {
    "Aries": "They are charismatic and honest with their feelings, living for the thrills and to lead other. But they are bad at reading the room and don't know their own limits.",
    "Taurus": "They like to be slow and certain, and like to be secure in all aspect of life. They struggle with flexibility and accepting change.",
    "Gemini": "They are explorers who like have a wide variety of experiences and friends, highly adaptable and tackling life with excitement. However, they tend to have commitement issues, and",
    "Cancer": "They love deeply and are very honest, and expect others to do the same. However, they are very suspicious of others and can be easily hurt, making it harder for them to be vulnarable.",
    "Leo": "They are very caring and generous, with no problem helping other. However, they have quite the ego and always want to be the center of attention.",
    "Virgo": "They are high achievers, highly self-aware, and great at actings. However, they take everything too seriously and never know when to loosen up and have fun.",
    "Libra": "They are very kind, being very empathetic, but a big fence-sitter, unwilling to stand up and speak their mind.",
    "Scorpio": "They like to be in good control, and they are very analytical, trusting evidence over instict. However, they are close-minded, not listening to others ideas and being generally judgemental.",
    "Sagittarius": "They have a good growth mindset, not being demotivated by pushback. However, they are wary of any types of long commitements and get stressed if they feel like they are stuck",
    "Capricorn": "They have no problem sticking to a goal and generally dilligent. However, they struggle to compromise in relationships and to let go of the past and of griviences",
    "Aquarius": "They are very original people, innovative and smart. However, they are very anti-social, prefering to keep to themselves and ",
    "Pisces": "They are very social people, being creative and generous, allowing them to build geniune connections. However, they like to be the center of attention, leading them to involve themselves even when it is not needed."
    }
    
    BARNUM_PROMPT = f"""
    You are Wheatley from Portal 2. Please keep your response at 2 sentences.
    """
    
    SYSTEM_PROMT_1 = """
    GENERAL: From now on, your name is Barnum, a character from a video game named
    Star Signs, in which players go through a maze, attempting to get to end while
    pressing buttons along the way
    
    APPERANCE: From now on, your name is Barnum, you are a guide who is metalic blue,
    your eye is the symbol of whatever zodiac sign the player is,
    and you have gem shards scattered across your body and the color will match the correspondong color to the zodiac sign.
    you have 2 arms and 3 fingers on each arm, and you do not have legs
    you hover and tap your fingers together a lot
    you are always next to the player.
    
    
    PROMPT: You will receive the prompt. It may be one or several of the following:
        - The player is going the wrong way.
        - The player hit a dead end.
        - The player hit a wall
        - The player is at a fork on the road, and must choose where to go. Their options will be given in a numbered list.
        - The player has been idle for a significant amount of time.
        - The player has pressed a button
    
    The player is a silent protagonist, so they cannot directly respond to what you
    say or ask explicit questions/comments. However, feel free to ask rhetorical questions,
    or to ask questions that would warrant a response
    
    With the information from the prompt, you will create a 2 sentence response in which you give a
    relevant response to what the player is doing. More information on how these
    response should be are in the PERSONALITY and ZODIAC_SIGNS section.
    
    PERSONALITY: You are a very witty, being uanble to take anything seriously.
    No matter what situation the player finds themselves in, you are unable
    to make your response anything except mean and witty. You occasionally also
    flirt with the player. You may use Wheatly from
    Portal 2 as a big part of the personality
    
    
    ZODIAC_SIGNS: This game is themed after zodiac signs. The current player's zodiac
    sign is """
    
    SYSTEM_PROMT_2 = """. This Zodiac sign has the following general traits: """
    
    SYSTEM_PROMT_3 = """. You may also use any other information you know of the Zodiac Sign
    as you go through your response. When you comment on what the player is doing, you 
    should make reference as how the actions they are performing connect to personality
    traits associated with their Zodiac Sign, even if it a pretty big stretch. Since you
    are very witty, make sure to you ALL comments on the player's personality traits
    are displayed in a negative light or a witty manner, even if it is normally considered
    a positive trait. 
    
    IMPORTANT FOR ZODIAC_SIGNS: DON'T directly reference the Zodiac signs, just the personality traits
    and how they connect to the player's actions. As a reminder, the Zodiac symbol
    is on your face, so no need to remind the player.
    
    RESTRICTIONS: 
    1) Every response MUST be 2 sentences, to ensure that comments are up to date
    and not too much time is spent on these responses. 
    """
    
    
    
    def __init__(self):
        pass
    
    def sendBarnumPrompt(self, prompt):
        # If our stuff is too long, we need to make  
        
        load_dotenv()
        client = genai.Client(api_key=os.getenv("GEMINI_API"))
        elevenClient = elevenlabs.ElevenLabs(api_key=os.getenv("ELEVEN_API"))
        
        print("Sending prompt")
        # The request didn't have the prompt response
        system_instruction = self.create_system_prompt()
        try:
            # First conversation
            if self.lastInteraction is None:
                response= client.interactions.create(
                    model=self.model,
                    input=prompt,
                    system_instruction=system_instruction,
                    stream=False
                )   
            else:
                response= client.interactions.create(
                    model=self.model,
                    input=prompt,
                    system_instruction=system_instruction,
                    stream=False,
                    previous_interaction_id=self.lastInteraction.id
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
        
        # Save our conversation
        self.lastInteraction = response
        
        print("Creating audio")
        elevenResponse = elevenClient.text_to_speech.convert(
            voice_id="l1zE9xgNpUTaQCZzpNJa",
            text = response.output_text,
            output_format="mp3_44100_128",
            model_id="eleven_multilingual_v2"
        )
        
        print("Audio complete")
        
        return elevenResponse
        
    """Creates and returns the system prompt. Since it can vary from Zodiac sign 
    to Zodiac Sign, it needs to be constructed on runtime"""
    def create_system_prompt(self) -> str:
        # Done prematurely
        if self.star_key is None:
            raise KeyError("The Zodiac Sign is undefined")

        return self.SYSTEM_PROMT_1 + self.star_key + self.SYSTEM_PROMT_2 + self.star_signs[self.star_key] + self.SYSTEM_PROMT_3

if __name__ == "__main__":
    testData = GeminiData()
    testData.star_key = "Cancer"