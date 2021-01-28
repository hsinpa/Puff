import {Schema} from 'mongoose';

const FriendSchema = new Schema ({
    requester: { type: Schema.Types.ObjectId, ref: 'puff_accounts'},
    recipient: { type: Schema.Types.ObjectId, ref: 'puff_accounts'},
    status: {
        type: Number,
        enums: [
            0,    //'friends',
            1,    //'pending',
            2     //'block',
        ]
    }
});

export default FriendSchema;