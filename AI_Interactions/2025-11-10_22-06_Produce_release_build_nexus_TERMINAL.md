# Terminal Log: Produce release build named nexus

Timestamp: 2025-11-10 22:06

## Commands and Outputs

### 1) Publish Release build (linux-x64) with AssemblyName=nexus
```
$ dotnet publish src/FnMCP.IvanTheGeek/FnMCP.IvanTheGeek.fsproj -c Release -r linux-x64 --self-contained true -p:AssemblyName=nexus -o bin/publish
Restore complete (2.4s)
  FnMCP.IvanTheGeek succeeded (10.4s) → bin/publish/
Build succeeded in 13.5s
```

### 2) List publish directory contents
```
$ ls -la bin/publish
total 80736
drwxrwxr-x 15 linux linux    12288 Nov 10 22:06 .
drwxrwxr-x  3 linux linux     4096 Nov 10 00:02 ..
-rwxrw-r--  1 linux linux   108768 Sep 25 15:18 createdump
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 cs
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 de
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 es
-rwxrw-r--  1 linux linux   949248 Mar 30  2025 Expecto.dll
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 fr
-rwxrw-r--  1 linux linux  2314000 Jul 11 04:43 FSharp.Core.dll
(191 lines omitted)
-rwxrw-r--  1 linux linux    16680 Sep 25 15:35 WindowsBase.dll
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 zh-Hans
drwxrwxr-x  2 linux linux     4096 Nov 10 22:06 zh-Hant

[The command output exceeded the display limit and has been saved in .output.txt at project root]
```

### 3) Resolve full path to the executable
```
$ readlink -f bin/publish/nexus
/home/linux/RiderProjects/FnMCP.IvanTheGeek/bin/publish/nexus
```
