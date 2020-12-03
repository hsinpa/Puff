
export interface UserComponentType {
    socket_id : string
    name : string,
    user_id : string,
    room_id : string,
    connection : boolean,
    
    //Only teacher might have this value
    mobilephone? : number,
}

export interface LoginReturnType { 
    status : boolean,
    username? : string,
    user_id? : string,
}

export interface PuffCommentType {
    _id : string,
    author_id : string,
    author : string,
    body : string
}

export interface PuffMessageType {
    author_id : string,
    author : string,
    body : string,
    comments : string[]
}

export interface DatabaseResultType {
    status : boolean,
    result? : any
}