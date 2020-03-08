# Project     : Team DHL Capstone
# File        : matchQuestion.py
# Author      : Ricardo Mohammed
# Date        : February 18th, 2020
# Description : This script takes a user-entered question and matches it to the most likely match
#               from the compiled list of standard culture questions.

import os
import sys
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

# Import the Universal Sentence Encoder module.

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

# Function    : embed
# Parameters  : input - The set of culture questions.
# Description : Uses the Universal Sentence Encoder model to find the embeddings of the input.
#       
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

    # print('The strength of the match is: ')
    # print(maxMatch)
    # print('The index of the match is: ')
    # print(maxIndex)
    return maxIndex


# Read the questions from the file.
with open('Assets\cultureQuestions.csv', newline='') as questionsFileCSV:
    questionReader = csv.reader(questionsFileCSV, delimiter=',', quotechar='|')
    for cultureQuestionSet in questionReader:
        pass

# Get the embeddings for the list of culture questions. The embeddings pertain to meaning.


nationalityQuestion = sys.argv[1]
nationalityQuestionArray = [nationalityQuestion]

cultureQuestionEmbeddings = embed(cultureQuestionSet)
nationalityQuestionEmbeddings = embed(nationalityQuestionArray)


cultureQuestionEmbeddingsList = np.array(cultureQuestionEmbeddings).tolist()

# print('The message embeddings: ')
# print(np.array(cultureQuestionEmbeddings).tolist()[0])
print(cultureQuestionSet[compare_questions(nationalityQuestionEmbeddings, cultureQuestionEmbeddingsList)])


# ------------- Print Data to Verify the Question Set -------------
# print('The type and items in cultureQuestionSet:')
# print(type(cultureQuestionSet))
# print(cultureQuestionSet)

# print('The count of items in the culture question set:')
# print(len(cultureQuestionSet))

# print('The length of the culture question set: ')
# print(len(cultureQuestionSet))
# print(cultureQuestionSet)
# -----------------------------------------------------------------