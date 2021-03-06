namespace ToDoAPI

///Facade to access the read model of the application
module ReadModel =

    ///Internal store of the read model
    let private items = ResizeArray<TaskModel>()
    items.Add({ id = 1; title = "Task 1"; isDone = true })
    items.Add({ id = 2; title = "Task 2"; isDone = true })
    items.Add({ id = 3; title = "Task 3"; isDone = true })

    ///<summary>
    ///Loads the complete read model
    ///</summary>
    ///<returns>All items from the read model</returns>
    let getAll =
        items

    ///<summary>
    ///Loads all items from the read model that apply to the specified <c>filter</c>
    ///</summary>
    ///<param name="filter">Predicate to apply on the read model</param>
    let get filter =
        items |> Seq.filter filter

    ///<summary>
    ///Loads all items from the read model that apply to the specified <c>filter</c>
    ///</summary>
    ///<param name="filter">Predicate to apply on the read model</param>
    let getFirst filter =
        items |> Seq.find filter

    ///<summary>
    ///Adds the specified <c>task</c> to the read model
    ///</summary>
    ///<param name="task">Task to insert</param>
    let insertTask task =
        items.Add(task) |> ignore
    
    ///<summary>
    ///Returns the index of the Task with the specified <c>id</c>
    ///</summary>
    ///<param name="id">Task ID to find</param>
    ///<returns>Option with the index of the task with the specified <c>id</c>; otherwise <c>None</c></returns>
    let private findIndexById id =
        let index = items.FindIndex(fun i -> i.id = id)
        if index = -1 then None else Some(index)

    ///<summary>
    ///Sets the specified <c>newTask</c> in the read model instead of the existing one
    ///</summary>
    ///<param name="newTask">New task to set</param>
    let updateTask newTask =
        match (findIndexById newTask.id) with
        | Some(i) -> items.Item(i) <- newTask
        | None    -> "Task not found" |> ignore

    ///<summary>
    ///Deletes the task with the specified <c>id</c> from the read model
    ///</summary>
    ///<param name="id">ID of the task to delete</param>
    let deleteTask id =
        match findIndexById id with
        | Some(i) -> items.RemoveAt(i) |> ignore
        | None -> "Task not found" |> ignore
