
export interface UserComponentType {
    socket_id : string
    name : string,
    user_id : string,
    room_id : string,
    connection : boolean,
    
    //Only teacher might have this value
    mobilephone? : number,
}

export interface ClientSignLogType { 
    password : string,
    username : string,
    email : string,
    type : number,
    auth_key? : string
}

export interface AccountType {
    _id : string,
    username : string,
    email : string
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
    status : number,
    result? : any
}