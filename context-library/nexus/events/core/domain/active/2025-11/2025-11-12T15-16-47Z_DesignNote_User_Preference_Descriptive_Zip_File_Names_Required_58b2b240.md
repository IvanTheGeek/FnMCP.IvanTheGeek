---
id: 8f55e8b5-3c99-479a-b2b5-7983fc217f92
type: DesignNote
title: "User Preference: Descriptive Zip File Names Required"
summary: "Added user preference: zip files must have descriptive names reflecting contents, never generic \"file.zip\". Applies to all chats."
occurred_at: 2025-11-12T10:16:47.759-05:00
tags:
  - user-preference
  - file-naming
  - downloads
  - organization
---

## User Preference: Descriptive Zip File Names

**Requirement:** All zip files created for download must have descriptive names reflecting their contents.

## Examples

**Good:**
- `nexus-migration-files.zip`
- `laundrylog-v7-design.zip`
- `phase6-completion-checklist.zip`
- `perdiem-documentation-2025-11.zip`

**Bad:**
- `file.zip`
- `archive.zip`
- `download.zip`

## Rationale

- User needs to identify files in Downloads folder
- Multiple zip files with generic names cause confusion
- Descriptive names = better organization
- Applies to all chats (project and non-project)

## Application

This applies to any zip file creation in any conversation:
- Documentation packages
- Code bundles
- Migration scripts
- Design assets
- Any downloadable archive

**Memory updated:** Added to user preferences as memory edit #10.
