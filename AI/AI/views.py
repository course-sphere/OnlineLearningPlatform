"""
AI routes: hướng dẫn làm bài tập & tạo bài tập
"""

from flask import request, jsonify
from AI import app
import requests
OLLAMA_URL = "http://localhost:11434/api/generate"
def call_ollama(prompt):
    payload = {
        "model": "llama3",
        "prompt": prompt,
        "stream": False,
        "options": {
            "temperature": 0.3,
            "num_ctx": 4096
        }
    }

    res = requests.post(OLLAMA_URL, json=payload, timeout=60)
    return res.json()["response"]

@app.route('/ai/guidance', methods=['POST'])
def ai_guidance():
    data = request.json

    prompt = f"""
You are an academic tutor.

Student question:
{data['question']}

Lesson:
{data['lesson']['title']} - {data['lesson']['content']}

Resources:
{data['resources']}

Guide the student without answering the question.
"""

    response = call_ollama(prompt)

    return jsonify({
        "guidance": response
    })


@app.route("/ai/generate", methods=["POST"])
def generate_assignment():
    """
    AI hỗ trợ ra bài tập
    """
    data = request.json
    topic = data.get("topic")
    level = data.get("level", "beginner")

    if not topic:
        return jsonify({ "error": "Topic is required" }), 400

    response = f"""
    🔹 BÀI TẬP AI TẠO 🔹

    Chủ đề: {topic}
    Độ khó: {level}

    Yêu cầu:
    - Mô tả bài toán rõ ràng
    - Có input / output
    - Phù hợp với trình độ {level}

    Ví dụ:
    Hãy giải thích và áp dụng kiến thức về {topic}.
    """

    return jsonify({ "response": response })