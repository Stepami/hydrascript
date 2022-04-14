function positive(x: number): boolean {
    return x > 0
}

function negative(x: number): boolean {
    return x < 0
}

let x = 0
if (positive(x)) {
    print("positive")
} else if (negative(x)) {
    print("negative")
} else {
    print("zero")
}