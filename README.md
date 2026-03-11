# ModificationHistoryWriter

A Windows desktop tool that formats modification history entries from commit messages and writes them into the headers of source files.

## What it does

When working on a project that requires a change log inside each modified file, this tool automates the process:

1. You copy a commit message (e.g. `REQ1234 Fix login timeout`) to the clipboard.
2. The app formats it into a single-line history entry using your configured pattern.
3. You drop the files that were changed onto the app.
4. The app inserts the formatted line into the modification history block at the top of each file.

Example output line inserted into a file:

```
// 11.03.2026  John Doe  REQ1234          Fix login timeout
```

## Requirements

- Windows 10 or later
- .NET 8 runtime

## Installation

Build the solution in Visual Studio or with the .NET CLI:

```
dotnet build ModificationHistoryWriter.sln
```

Run the output from `ModificationHistoryWriterForm\bin\`.

## Configuration

The app reads its pattern from:

```
%AppData%\ModificationHistoryWriter\pattern.json
```

If the file does not exist, the app starts with an empty pattern.

### pattern.json structure

```json
{
  "Pattern": "// DATE  AUTHOR  TICKET          MESSAGE",
  "DateFormat": "dd.MM.yyyy",
  "Author": "John Doe",
  "TicketPattern": "((REQ|DEF)\\d*)\\s*(.*)"
}
```

| Field           | Description                                                                                   |
|-----------------|-----------------------------------------------------------------------------------------------|
| `Pattern`       | Template for the output line. Tokens `DATE`, `AUTHOR`, `TICKET`, `MESSAGE` are replaced.     |
| `DateFormat`    | .NET date format string applied to today's date, e.g. `dd.MM.yyyy`.                          |
| `Author`        | Your name as it should appear in the history line.                                            |
| `TicketPattern` | Regex applied to the clipboard text. Group 1 = ticket ID, group 3 = message description.     |

> Note: Accented characters in the author name are automatically converted to their ASCII equivalents.

## How to use

### Step 1 — Configure pattern.json

Create `%AppData%\ModificationHistoryWriter\pattern.json` with your project's format and your name (see above).

### Step 2 — Copy a commit message

Copy a line matching your `TicketPattern` to the clipboard, for example:

```
REQ1234 Fix login timeout
```

### Step 3 — Format

Click the **Format** button (arrow icon) in the toolbar. The formatted history line appears in the text box at the top of the window, and a toast notification confirms the result.

### Step 4 — Drop files

Drag and drop the source files you want to update onto the file list in the lower half of the window. Files can also be passed as command-line arguments when launching the app.

### Step 5 — Save

Click the **Save** button (disk icon). The app inserts the formatted line into each file immediately after the last existing history line (the line that matches `// ... [...] ...` pattern followed by an empty line).

### Additional toolbar buttons

| Button       | Action                                                                        |
|--------------|-------------------------------------------------------------------------------|
| Format       | Reads clipboard, applies pattern, shows result in the text box.              |
| Copy         | Copies the last formatted line back to the clipboard.                        |
| Clear        | Clears the file list.                                                        |
| Save         | Writes the formatted line into all files in the list.                        |
| Show Pattern | Displays the currently loaded pattern settings in the file list and a toast. |

## File header format expected

The **Save** operation looks for an existing modification history block in the file. It finds the last line that looks like a history comment (matching `// ... [...] ...`) followed by an empty line, and inserts the new entry there.

Example of a compatible file header:

```
// MODIFICATION HISTORY
// -----------------------------------------------------------------------------
// 10.03.2026  John Doe  REQ1000          Initial implementation
// 11.03.2026  John Doe  REQ1234          Fix login timeout
                                          <- new entry inserted here
```

If no such block exists in the file, no changes are made.

## Project structure

```
ModificationHistoryWriter/          Core library (net8.0)
ModificationHistoryWriterForm/      WinForms application (net8.0-windows)
ModificationHistoryWriter.Test/     Unit tests (xUnit)
```
