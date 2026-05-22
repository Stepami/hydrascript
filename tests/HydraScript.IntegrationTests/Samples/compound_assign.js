let x = {
    prop: 2;
}
x.prop += 4 + 5 * 3
x.prop *= 7
>>> x

let y = 1
y -= (y + 2) * (3 + 4)
y /= 5
>>> y

let arr: number[] = []
let i = 0
while (i < 5) {
    arr ++= [i]
    i += 1
}
>>> arr