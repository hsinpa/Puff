
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
    title : string,
    type : number,
    duration : number,
    privacy : number,

    latitude : number,
    longitude : number,
    images : string[],
    comments : string[],

    date : Date,
    expire : Date
}

export interface DatabaseResultType {
    status : number,
    result? : any
}

export enum Duration {
    Day = 0, Week, Month
}

export enum FriendStatus {
    Friend = 0,
    Invite, Receive, Block
}

export interface FriendComponentType {
    account: string,
    recipient: string,
    status: number
}