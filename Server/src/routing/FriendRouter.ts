
import * as path from 'path';
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';
import {PuffMessageType, ClientSignLogType} from '../Utility/Flag/TypeFlag';
import bodyParser = require('koa-bodyparser');

export let FriendRouter = function (router : Router, mongodb:MongoDB) {
    router.get('/friends/:account_id', async function (ctx:any, next:any) {
        let r = await mongodb.friendModel.GetFriends(ctx.params.account_id);
        
        ctx.body = "dfdf";
    });

    router.post('/friends/request_friend/', async function (ctx:any, next:any) {
        await ctx.render('index', {title: "HSINPA"});
    });

    router.post('/friends/response_friend/', async function (ctx:any, next:any) {
        await ctx.render('index', {title: "HSINPA"});
    });
}