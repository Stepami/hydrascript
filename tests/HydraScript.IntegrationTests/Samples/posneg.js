function positive(x: number): boolean {
    return x > 0
}

function negative(x: number): boolean {
    return x < 0
}

let x = 0
if (positive(x)) {
    >>>"positive"
} else if (negative(x)) {
    >>>"negative"
} else {
    >>>"zero"
}