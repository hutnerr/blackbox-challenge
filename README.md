## Overview

Black Box Challenge is a terminal-based function puzzle game where players attempt to reverse-engineer hidden functions. By providing inputs and observing outputs, users must deduce the underlying logic and recreate the function using a custom-built programming language and interpreter.

This project involves building a full interpreter from scratch, including a lexer, parser, and evaluator. It also features an interactive terminal user interface that allows users to test and refine their solutions in real time.

**Tech Stack:**
- Custom Programming Language (Lexer & Parser)
- [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) for TUI
- Abstract Syntax Trees & Visitor Pattern
- Recursive Descent Parsing & Tree Traversal

## Showcase

Black Box Challenge delivers an interactive coding experience entirely within the terminal, combining elements of game design with compiler construction.

**Key Features:**
- Custom programming language supporting arithmetic, logical, and bitwise operations  
- Full interpreter with lexer, parser, and evaluator  
- Terminal-based UI with live code editing  
- Parameter testing table to compare expected vs. actual outputs  
- Real-time syntax error reporting  
- Support for loading custom mystery functions from files  

**How It Works:**
1. Observe input-output behavior of a hidden function  
2. Write code in the custom language to replicate the function  
3. Test your implementation against multiple cases  
4. Refine your solution until outputs match expected results

![](https://www.hunter-baker.com/resources/images/projects/blackbox-challenge.png)
