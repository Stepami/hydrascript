type int = number
type ints = int[]

function swap(array: ints, i: int, j: int) {
    let ai = array[i]
    array[i] = array[j]
    array[j] = ai
}

function partition(values: ints, l: int, r: int): int {
    let x = values[r]
    let less = l

    let i = l
    while (i < r) {
        let value = values[i]
        if (value <= x) {
            swap(values, i, less)
            less = less + 1
        }
        i = i + 1
    }
    swap(values, less, r)
    return less
}

function quickSortImpl(values: ints, l: int, r: int) {
    if (l < r) {
        let q = partition(values, l, r)
        quickSortImpl(values, l, q - 1)
        quickSortImpl(values, q + 1, r)
    }
}

function quickSort(values: ints, n: int) {
    if (n != 0) {
        quickSortImpl(values, 0, n - 1)
    }
}

let numbers = [6,2,4,3,7,1,5,]
quickSort(numbers, 7)
print(numbers as string)