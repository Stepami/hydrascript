const obj = {
    arr: [1 + 1, 1 - 1];
}
const negSizeStr = -(~obj.arr) as string

>>> negSizeStr

const correctCast = negSizeStr == "-2"
let num = correctCast ? -2 : 0
if (correctCast) {
    let single = [num]
    let x = single[0] = num = obj.arr[2 - 2]
    >>> x
}