namespace ToDoAPI

///Business logic to apply to incoming commands
module DomainLogic =

    ///<summary>
    ///Processes the specified new <c>task</c>
    ///</summary>
    ///<param name="task">New task to process</param>
    ///<returns>Enumeration of events that occured while creating a new task</returns>
    let createTask task =
        let nextID = (ReadModel.getAll |> Seq.length) + 1
        seq [ TaskAddedEvent { id = nextID; title = task.title; isDone = task.isDone } ]

    let private getOldTask id =
        ReadModel.getFirst (fun task -> task.id = id)

    ///<summary>
    ///Processes the update of the <c>title</c> of the task with the specified <c>id</c>
    ///</summary>
    ///<param name="(id, title)">Tuple with the <c>id</c> of the task to update and the new <c>title</c> to set</param>
    ///<returns>Enumeration of events that occured while updating the title of an existing task</returns>
    let updateTaskTitle (id, title) =
        let oldTask = getOldTask id
        let newTask = { id = id; title = title; isDone = oldTask.isDone }
        seq [ TaskUpdatedEvent newTask ]

    ///<summary>
    ///Processes the completion of the task with the specified <c>id</c>
    ///</summary>
    ///<param name="id"><c>id</c> of the task to complete</param>
    ///<returns>Enumeration of events that occured while completing an existing task</returns>
    let updateTaskStatus (id, status) =
        let oldTask = getOldTask id
        let newTask = { id = id; title = oldTask.title; isDone = status }

        match status with
        | true -> seq [ TaskCompletedEvent newTask ]
        | false -> seq [ TaskUncompletedEvent newTask ]

    ///<summary>
    ///Processes the deletion of the task with the specified <c>id</c>
    ///</summary>
    ///<param name="id"><c>id</c> of the task to delete</param>
    ///<returns>Enumeration of events that occured while deleting an existing task</returns>
    let deleteTask id =
        seq [ TaskDeletedEvent id ]

    ///<summary>
    ///Processes the deletion of all completed tasks
    ///</summary>
    ///<returns>Enumeration of events that occured while deleting all completed task</returns>
    let deleteCompletedTasks =
        let filterByStatus task = task.isDone
        let createDeletedEvent task = TaskDeletedEvent task.id

        (ReadModel.get filterByStatus) |> ((Seq.map createDeletedEvent) >> List.ofSeq >> Seq.ofList)
