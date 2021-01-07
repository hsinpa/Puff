import {createHash} from 'crypto';

export function RollDice() {
    return Math.floor((Math.random() * 1.5) );
};

export function RandomRange(min : number, max : number) {
  return ~~(Math.random() * (max - min + 1)) + min
};

export function GenerateRandomString(p_len : number) {
  const shaString = createHash('sha1').update(new Date().toJSON()).digest('hex');

  if (p_len > shaString.length - 1)
    p_len = shaString.length - 1;

  return shaString.substring(0, p_len);
};

export function SHA256Hash(p_input : string) {
  return createHash('sha256').update(p_input).digest('base64');
}

export function GetDate(extend_date : number) {
  return new Date(Date.now() - extend_date*24*60*60*1000);
}
