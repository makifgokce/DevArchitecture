export class Token {
    success!:boolean;
    message!:string;
    data!:Data;
}
export class Data {
    expiration!:string;
    token!:string;
    claims!:string[];
    roles!:string[];
    refreshToken!:string;
}