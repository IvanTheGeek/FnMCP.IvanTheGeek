# Junie Guidelines

## Session Interaction Logging

### Directory Structure
- Create an `AI_Interactions` directory in the repository root (if not already present)
- All interaction logs are stored in this directory

### Per-Prompt Documentation
For each prompt received from the user:

1. **Create a new markdown file** with the following naming convention:
   - Format: `YYYY-MM-DD_HH-mm_Brief_Summary.md`
   - Use local system time (timezone not critical)
   - Replace spaces in summary with underscores
   - Keep summary concise (3-6 words recommended)
   - Ensure filename is valid for Linux, macOS, and Windows (max 255 characters)
   - Example: `2025-11-01_13-23_Create_Junie_guidelines_file.md`

2. **File contents must include**:
   - The original user prompt (complete and unmodified)
   - All Junie outputs and responses
   - Any clarifying questions asked by Junie
   - All user answers to Junie's questions
   - Continue documenting until the prompt response is fully completed

3. **Git Commit Requirement**:
   - Upon completion of the prompt response, you MUST explicitly create a git commit
   - This is not automatic - you must execute the git commands yourself
   - Commit message should include:
     - Title: Brief description of what was accomplished
     - Description: More detailed explanation of changes/actions taken
   - Commit should include:
     - The new interaction log file(s)
     - Any files created/modified during the session

4. **Create a terminal log file** (companion to the main interaction log):
   - Format: `YYYY-MM-DD_HH-mm_Summary_TERMINAL.md`
   - Same base name as the main interaction log file, with `_TERMINAL` added before `.md`
   - Example: `2025-11-01_13-23_Create_Junie_guidelines_file_TERMINAL.md`
   - **File contents must include**:
     - All terminal commands executed during the session
     - Complete terminal output as reported when user clicks OPEN for terminal commands
     - Both successful command outputs and any errors encountered
     - Timestamps or sequence information if relevant
   - This file captures the technical execution details separate from the conversation flow

### Git Repository Initialization
When initializing new git repositories:
- Always use `main` as the default branch name
- Never use `master` as the default branch
- Command: `git init -b main` or `git init && git branch -m main`

## Workflow Summary

For each user prompt:
1. Process and complete the user's request
2. Create `AI_Interactions/YYYY-MM-DD_HH-mm_Summary.md` with full interaction log
3. Create `AI_Interactions/YYYY-MM-DD_HH-mm_Summary_TERMINAL.md` with all terminal commands and outputs
4. Verify files need committing: Execute `git status` to confirm new/modified files
5. Stage files: Execute `git add` for the interaction log files and any work artifacts
6. Verify staging: Execute `git status` again to confirm files are staged
7. Commit: Execute `git commit` with descriptive title and detailed message
8. Verify commit: Check that commit succeeded (look for commit SHA in output)
9. Ensure commit includes both the interaction log files and any work artifacts

## File Naming Guidelines

**Valid characters**: Letters, numbers, underscores, hyphens  
**Avoid**: Special characters that are problematic on any filesystem (`, *, ?, <, >, |, :, ", \, /)  
**Maximum length**: Keep total filename under 200 characters for cross-platform compatibility  
**Readability**: Use underscores to separate words in the summary portion
