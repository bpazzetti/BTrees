# BTrees

**BTrees** is a comprehensive implementation of various B-tree data structures, including B-tree, B* Tree, and B+ Tree. This project aims to provide a implementations of these tree structures for educational and practical purposes.

## Features

- **B-tree Implementation**: Standard B-tree data structure with insertion, deletion, and search operations.
- **B* Tree Implementation**: Enhanced B-tree with improved space utilization and performance.
- **B+ Tree Implementation**: B-tree variant with all values at the leaf level, providing efficient range queries.

## Technologies

This project is built using:

- **C#**: The primary programming language used for the implementation.

## Installation

To install and run this project, follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/bpazzetti/BTrees.git
   ```
3. Navigate to the project directory:
   ```
   cd BTrees
   ```
5. Open the project in Visual Studio and restore dependencies.

## Usage

To use this project, follow these steps:

1. Open the solution in Visual Studio.
2. Build the solution to ensure all dependencies are resolved.
3. Run the project to see the B-tree implementations in action.

Example usage:
```c#
// Example of creating and using a B-tree 
BTree bTree = new BTree(3); 
bTree.Insert(10); 
bTree.Insert(20); 
bTree.Insert(5); 
bTree.Search(10); // Returns true 
bTree.Delete(10); 
bTree.Search(10); // Returns false
```
