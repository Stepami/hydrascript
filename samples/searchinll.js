type node = {
    data: number;
    next: node;
}

let head: node = {
    data: 1;
    next: null;
}

let h1: node = {
    data: 2;
    next: null;
}

head.next = h1

function searchEven(n: node): number {
    let h = n
    let d = -1
    while (h != null) {
        d = h.data
        print(d as string)
        if (d % 2 == 0) {
            return d
        }
        h = h.next
    }
    return d
}

let j = searchEven(head)
print(j as string)