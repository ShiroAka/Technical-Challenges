# Word Finder Dev Challenge

## Introduction
This repo contains different algorithms to find words in a char 2D matrix based on a list of words.
The algorithms count all the occurrences of each word in the matrix and return a list of the words (from the original list) that were found in the matrix, ordered by their frequency in the matrix.

The algorithms only search words horizontally or vertically.

The solution contains only 2 algorithms (so far):
  - A slower one which does a secquential search (checks if each word is in the matrix by iterating through all the columns and rows)
  - A faster algorithm which implements a "Trie" structure and a Depth-First Search

The solution also contains some tests to check each algorithm and how much time each one takes when using a 64x64 matrix and long words (the challenge mentions the matrices would be 64x64 at most).

More tests will be added in the future...


## Folder Structure
The "Docs" folder contains the original challenge description and a document explaining assumptions (based on the given description) and ideas used to develop the algorithms

The "src" folder contains the code to develop the algorithms.

