import {Schema} from 'mongoose';

const FriendSchema = new Schema ({
    account: { type: Schema.Types.ObjectId, ref: 'puff_accounts'},
    recipient: { type: Schema.Types.ObjectId, ref: 'puff_accounts'},
    status: {
        type: Number,
        enums: [
            0,    //'friends',
            1,    //'request friend',
            2,    //'receive request',
            3     //'block',
        ]
    }
});

export default FriendSchema;