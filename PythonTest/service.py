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
import seaborn as sns
import csv
import sentencepiece as spm

# Read the questions from the file.
with open('cultureQuestions.csv', newline='') as questionsFileCSV:
    questionReader = csv.reader(questionsFileCSV, delimiter=',', quotechar='|')
    for cultureQuestionSet in questionReader:
        pass
        
    
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
        self.request.sendall(bytearray(setup_model(self.request.recv(1024).strip()), 'utf-8'))

def embed(messages):
    values, indices, dense_shape = process_to_IDs_in_sparse_format(sp, messages)
    with tf.Session() as session:
        session.run([tf.global_variables_initializer(), tf.tables_initializer()])
        return  session.run(
            encodings,
            feed_dict={input_placeholder.values: values,
                        input_placeholder.indices: indices,
                        input_placeholder.dense_shape: dense_shape})

    
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

def setup_model(nationalityQuestion):
    return cultureQuestionSet[compare_questions(embed([nationalityQuestion]), cultureQuestionEmbeddingsList)]


    

if __name__ == "__main__":
    cultureQuestionEmbeddingsList = np.array(embed(cultureQuestionSet)).tolist()

    # print("\n\n\n\n\n\n\n\n\n\n\")
    # while True:
    #     user_input = input("Enter question: ")
    #     if user_input == "quit":
    #         break
    #     print("Answer        : " + setup_model(user_input))
    #     print("\n")

    HOST, PORT = socket.gethostname(), 11000

    server = socketserver.TCPServer((HOST, PORT), MyTCPHandler)
    server.serve_forever()
    main()