type vec2d = {
    x: number;
    y: number;
}

function makeVec(x: number, y: number): vec2d {
    return {
        x: x;
        y: y;
    }
}

function vlensquared(v: vec2d): number {
    const x = v.x
    const y = v.y
    return x * x + y * y
}

let v = makeVec(3, 4)
let l = vlensquared(v)
print(l as string)
l = v.vlensquared()
print(l as string)