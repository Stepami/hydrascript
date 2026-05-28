function f() {
    let i = 0
    while (true) {
        if (i == 5 && i != 6) {
            return
        }
        i += 1
    }
}

f()