// square root

function abs(x: number): number {
    return x < 0 ? -x : x
}

function sqrt(num: number): number {
    let lastGuess: number, guess = num / 3

    lastGuess = guess
    guess = (num / guess + guess) / 2
    while ((abs(lastGuess - guess)) > 0.000000000000005)
    {
        lastGuess = guess
        guess = (num / guess + guess) / 2
    }

    return guess
}

>>> sqrt(3)