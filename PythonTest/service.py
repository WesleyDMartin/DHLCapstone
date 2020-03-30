import socketserver
import socket
import os
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '3' 
import tensorflow.compat.v1 as tf
tf.disable_v2_behavior() 
tf.disable_resource_variables()
import tensorflow_hub as hub
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import re
import json
import requests
import seaborn as sns
import csv
import sentencepiece as spm

cultureQuestionEmbeddingsList = []
culturalQuestionSet = []
responseJsonValue = []
    
model = hub.Module("https://tfhub.dev/google/universal-sentence-encoder-lite/2")

input_placeholder = tf.sparse_placeholder(tf.int64, shape=[None, None])
encodings = model(
    inputs=dict(
        values=input_placeholder.values,
        indices=input_placeholder.indices,
        dense_shape=input_placeholder.dense_shape))

with tf.Session() as sess:
    spm_path = sess.run(model(signature="spm_path"))

sp = spm.SentencePieceProcessor()
sp.Load(spm_path)

class MyTCPHandler(socketserver.BaseRequestHandler):

    def handle(self):
        self.request.sendall(bytearray(process_command(self.request.recv(1024).strip()), 'utf-8'))

def embed(messages):
    values, indices, dense_shape = process_to_IDs_in_sparse_format(sp, messages)
    with tf.Session() as session:
        session.run([tf.global_variables_initializer(), tf.tables_initializer()])
        return  session.run(
            encodings,
            feed_dict={input_placeholder.values: values,
                        input_placeholder.indices: indices,
                        input_placeholder.dense_shape: dense_shape})


def set_culture(culture):
    global cultureQuestionEmbeddingsList
    global culturalQuestionSet
    global responseJsonValue
    # BASE_URL = "https://nameless-eyrie-58237.herokuapp.com/"
    # BASE_URL = "http://192.168.0.117:3000/"
    parameters = {"culture": culture[:-5]}
    response = requests.get("http://69.165.169.118:3000/questions", params = parameters)
    culturalQuestionSet = []
    responseJsonValue = response.json()
    for q in responseJsonValue:
        culturalQuestionSet.append(q["value"])
    cultureQuestionEmbeddingsList = np.array(embed(culturalQuestionSet)).tolist()


    
def process_to_IDs_in_sparse_format(sp, sentences):
    # An utility method that processes sentences with the sentence piece processor
    # 'sp' and returns the results in tf.SparseTensor-similar format:
    # (values, indices, dense_shape)
    ids = [sp.EncodeAsIds(x) for x in sentences]
    max_len = max(len(x) for x in ids)
    dense_shape=(len(ids), max_len)
    values=[item for sublist in ids for item in sublist]
    indices=[[row,col] for row in range(len(ids)) for col in range(len(ids[row]))]
    return (values, indices, dense_shape)


# Function    : compare_question
# Parameters  :  
# Description : Compares the given question to the set of culture questions
#               and returns the closest matching question.
def compare_questions(question, questionSet):
    i = 0
    maxMatch = 0
    maxIndex = 0
    for q in questionSet:
        matchStrength = np.inner(question, q)
        if matchStrength > maxMatch: 
            maxMatch = matchStrength
            maxIndex = i
        i += 1
    return maxIndex


def process_command(command):
    s = command.decode()
    print(s)
    segs = s.split("|")
    if segs[0] == "SETCULTURE":
        set_culture(segs[1])
        return "sdfsdfdsfdsfdsfsdfdsfsf"
    else:
        return setup_model(segs[1].encode())

def setup_model(nationalityQuestion):
    print(len(cultureQuestionEmbeddingsList))
    match = culturalQuestionSet[compare_questions(embed([nationalityQuestion]), cultureQuestionEmbeddingsList)]
    for q in responseJsonValue:
        if q["value"] == match:
            return json.dumps(q)
    return ""

    

if __name__ == "__main__":
    HOST, PORT = socket.gethostname(), 11001

    server = socketserver.TCPServer((HOST, PORT), MyTCPHandler)
    server.serve_forever()
    main()