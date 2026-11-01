> This was a learning project and will not be updated anymore.

## Overview
Black Box Challenge is a terminal-based puzzle game where players reverse-engineer hidden functions by observing input-output behavior and recreating the logic in a custom-built programming language.

It was developed in C# with a partner and centers around building a complete interpreter from scratch: a custom lexer, a recursive descent parser, and an evaluator powered by abstract syntax trees and the Visitor pattern, all integrated into an interactive terminal interface.

**Tech Stack:**
- C#
- Custom Lexer & Parser
- Abstract Syntax Trees & Visitor Pattern
- Recursive Descent Parsing
- [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui)

> View more about the project on its [showcase page](https://www.hunter-baker.com/pages/projects/black-box-challenge.html).

## Features
- **Custom Language**: Supports arithmetic, logical, and bitwise operations
- **Full Interpreter**: Lexer, parser, and evaluator
- **Terminal UI**: Live code editing in the terminal
- **Test Table**: Compare expected vs. actual outputs across parameters
- **Error Reporting**: Real-time syntax errors
- **Custom Puzzles**: Load your own mystery functions from files

## Usage
1. Observe the input-output behavior of a hidden function
2. Write code in the custom language to replicate the function
3. Test your implementation against multiple cases
4. Refine your solution until the outputs match

## Showcase
<img src="https://www.hunter-baker.com/assets/projects/black_box_challenge/blackbox-challenge.png" alt="Black Box Challenge terminal interface">
