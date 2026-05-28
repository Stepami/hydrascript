const obj = {
    arr: [1 + 1, 1 - 1];
}
const negSizeStr = -(~obj.arr) as string

>>> negSizeStr

const correctCast = negSizeStr == "-2"
if (correctCast){
    let single = [0]
    let x = single[0] = obj.arr[2 - 2]
    >>> x
}