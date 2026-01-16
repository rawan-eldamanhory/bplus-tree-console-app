# B+ Tree Implementation in C#

This repository contains a clean implementation of a **B+ Tree** data structure using **C#**.

## Features
- Insert keys into the B+ Tree
- Search for a key
- Delete keys
- Automatic node splitting and merging
- Maintains balanced tree properties

## Tree Properties
- Order: configurable (default: 3)
- All data stored in leaf nodes
- Internal nodes store keys for navigation
- Leaf nodes are linked for efficient range queries (can be extended)

## Technologies
- C#
- Console Application
- Visual Studio

## Example Usage
```csharp
BPTree tree = new BPTree(3);
tree.Insert(10);
tree.Insert(20);
tree.Insert(30);

bool found = tree.Search(20);
```

## Notes
This implementation is intended for educational purposes, demonstrating how B+ Trees work internally.

## Future Improvements
- Generic type support
- Leaf node linking
- Unit tests
- Performance optimizations
  
