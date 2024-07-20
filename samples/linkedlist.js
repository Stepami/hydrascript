type node = {
    data: number;
    next: node;
}
type list = {
    head: node;
}

function append(lst: list, item: number) {
    let tail: node = lst.head
    while (tail.next != null) {
        tail = tail.next
    }
    tail.next = makeNode(item)
}

function getOdd(lst: list): number[] {
    let result: number[]
    let n = lst.head
    while (n != null) {
        if (n.data % 2 != 0) {
            let i = n.data
            result = result ++ [i]
        }
        n = n.next
    }
    return result
}

function makeNode(item: number): node {
    return {
        data: item;
        next: null;
    }
}

let linkedList: list = {
    head: makeNode(1);
}

linkedList.append(3)
linkedList.append(5)
linkedList.append(7)

print(linkedList as string)

let odds = linkedList.getOdd()
print(odds as string)