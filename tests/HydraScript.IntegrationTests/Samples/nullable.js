function indexOrNull(arr:number[], val:number): number?{
    const n = ~arr
    let i = 0
    while(i < n){
        if (arr[i] == val)
            return i
        i = i + 1
    }
    return null
}

>>> indexOrNull([1,2,3], 4)
>>> indexOrNull([1,2,3], 2)