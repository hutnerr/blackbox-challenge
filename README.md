## Overview

Black Box Challenge is a terminal-based function puzzle game where players reverse-engineer hidden functions by observing input-output behavior and recreating the logic using a custom-built programming language.

This project was developed in C# with a partner and centers around building a complete interpreter from scratch. It includes a custom lexer, a recursive descent parser, and an evaluator powered by abstract syntax trees and the Visitor pattern, all integrated into an interactive terminal interface.

**Tech Stack:**
- C#  
- Custom Lexer & Parser  
- Abstract Syntax Trees & Visitor Pattern  
- Recursive Descent Parsing  
- [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui)

## Showcase

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
