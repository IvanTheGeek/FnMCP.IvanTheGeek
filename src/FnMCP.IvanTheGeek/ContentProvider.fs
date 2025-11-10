module FnMCP.IvanTheGeek.ContentProvider

open FnMCP.IvanTheGeek.Types

// Content provider abstraction for different content sources
type IContentProvider =
    abstract member GetResource: uri:string -> Async<Result<Resource, string>>
    abstract member ListResources: unit -> Async<Resource list>

