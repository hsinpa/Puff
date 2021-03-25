
import * as Router from 'koa-router';
import MongoDB from '../service/MongoDB';

export let FriendRouter = function (router : Router, mongodb:MongoDB) {
    router.get('/friends/:account_id', async function (ctx:any, next:any) {
        let r = await mongodb.friendModel.GetFriends(ctx.params.account_id);
        
        ctx.body = r;
    });

    router.post('/friends/request_friend', async function (ctx:any, next:any) {
        let r = await mongodb.friendModel.RequestFriend(ctx.request.body.account_id, ctx.request.body.target_id ,ctx.request.body.auth_key);

        ctx.body = r;
    });

    router.post('/friends/accept_friend', async function (ctx:any, next:any) {
        let r = await mongodb.friendModel.AcceptFriend(ctx.request.body.account_id, ctx.request.body.target_id ,ctx.request.body.auth_key);

        ctx.body = r;
    });

    router.post('/friends/reject_friend', async function (ctx:any, next:any) {
        let r = await mongodb.friendModel.RejectFriend(ctx.request.body.account_id, ctx.request.body.target_id ,ctx.request.body.auth_key);

        ctx.body = r;
    });
}