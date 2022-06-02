module Exam

(* If you are importing this into F# interactive then comment out
   the line above and remove the comment for the line bellow.

   Do note that the project will not compile if you do this, but
   it does allow you to work in interactive mode and you can just remove the '='
   to make the project compile work again.

   Do not remove the line (even though that does work) because you may inadvertantly
   introduce indentation errors in your code that may be hard to find if you want
   to switch back to project mode.

   Alternative, keep the line as is, but load ExamInteractive.fsx into the interactive environment
   *)
(* module Exam2020 = *)

(* 1: Insertion sort *)

(* Question 1.1 *)

let rec insert x =
    function
    | [] -> [ x ]
    | l :: ls when x <= l -> x :: l :: ls
    | l :: ls -> l :: (insert x ls)

let rec insertionSort =
    function
    | [] -> []
    | x :: ls -> insert x (insertionSort ls)

(* Question 1.2 *)

let insertTail x ls =
    let rec aux acc =
        function
        | [] -> acc @ [ x ]
        | l :: ls when x <= l -> acc @ (x :: l :: ls)
        | l :: ls -> aux (acc @ [ l ]) ls

    aux [] ls

let insertionSortTail ls =
    let rec aux acc =
        function
        | [] -> acc
        | x :: ls -> aux (insert x acc) ls

    aux [] ls

(* Question 1.3 *)

(*
    Q: Why are the higher-order functions from the List library
    not a good fit to implement insert?

    A: The greatest problem here would be that you have to stop computations, so you dont wanna loop through the entire list if the right case is right in the beginning. You would have to start checking if both places are the right fit or something.
    *)

let insertionSort2 ls =
    List.fold (fun state x -> insert x state) [] ls

(* Question 1.4 *)

let insertBy f x lst =
    let rec aux acc =
        function
        | [] -> acc @ [ x ]
        | l :: ls when f x <= f l -> acc @ (x :: l :: ls)
        | l :: ls -> aux (acc @ [ l ]) ls

    aux [] lst

let insertionSortBy f ls =
    List.fold (fun state x -> insertBy f x state) [] ls

(* 2: Code Comprehension *)
let rec foo x =
    function
    | y :: ys when x = y -> ys
    | y :: ys -> y :: (foo x ys)

let rec bar x =
    function
    | [] -> []
    | xs :: xss -> (x :: xs) :: bar x xss

let rec baz =
    function
    | [] -> []
    | [ x ] -> [ [ x ] ]
    | xs ->
        let rec aux =
            function
            | [] -> []
            | y :: ys -> ((foo y >> baz >> bar y) xs) @ (aux ys)

        aux xs

(* Question 2.1 *)

(*

    Q: What are the types of functions foo,  bar, and baz?

    A: <Your answer goes here>


    Q: What do functions foo, bar, and baz do?
       Focus on what they do rather than how they do it.

    A: <Your answer goes here>


    Q: What would be appropriate names for functions
       foo, bar, and baz?

    A: <Your answer goes here>

    *)


(* Question 2.2 *)


(*
    The function foo generates a warning during compilation:
    Warning: Incomplete pattern matches on this expression.


    Q: Why does this happen, and where?

    A: <Your answer goes here>


    Q: For these particular three functions will this incomplete
       pattern match ever cause problems for any possible execution of baz?
       If yes, why; if no, why not.

    A: <Your answer goes here>

    *)

let foo2 _ = failwith "not implemented"

(* Question 2.3 *)

(*
    In the function baz there is a sub expression foo y >> baz >> bar y

    Q: What is the type of this expression

    A: <Your answer goes here>


    Q: What does it do? Focus on what it does rather than how it does it.

    A: <Your answer goes here>

    *)

(* Question 2.4 *)

let bar2 _ = failwith "not implemented"

(* Question 2.5 *)

let baz2 _ = failwith "not implemented"

(* Question 2.6 *)

(*

    Q: The function foo is not tail recursive. Why?

    A: <Your answer goes here>

    *)

let fooTail _ = failwith "not implemented"

(* 3: Rock Paper Scissors *)

(* Question 3.1 *)

type shape = unit (* replace unit with the correct type declaration *)
type result = unit (* replace unit with the correct type declaration *)

let rps _ = failwith "not implemented"

(* Question 3.2 *)

type strategy = (shape * shape) list -> shape

let parrot _ = failwith "not implemented"

let beatingStrat _ = failwith "not implemented"

let roundRobin _ = failwith "not implemented"

(* Question 3.3 *)

(*

    Q: It may be tempting to generate a function that calculates your
       point tuple after n rounds and then use Seq.initInfinite to
       generate the sequence. This is not a good solution. Why?

    A: <Your answer goes here>

    *)

let bestOutOf _ = failwith "not implemented"

(* Question 3.4 *)

let playTournament _ = failwith "not implemented"

(* 4: Revers Polish Notation *)

(* Question 4.1 *)

type stack = unit (* replace unit with the correct type declaration *)

let emptyStack = () (* replace () with the correct value *)

(* Question 4.2 *)

type SM<'a> = S of (stack -> ('a * stack) option)

let ret x = S(fun s -> Some(x, s))
let fail = S(fun _ -> None)

let bind f (S a) : SM<'b> =
    S (fun s ->
        match a s with
        | Some (x, s') ->
            let (S g) = f x
            g s'
        | None -> None)

let (>>=) x f = bind f x
let (>>>=) x y = x >>= (fun _ -> y)

let evalSM (S f) = f emptyStack

let push _ = failwith "not implemented"
let pop _ = failwith "not implemented"

(* Question 4.3 *)

let write str : SM<unit> =
    S (fun s ->
        printf "%s" str
        Some((), s))

let read =
    let rec aux acc =
        match System.Console.Read() |> char with
        | '\n' when acc = [] -> None
        | c when System.Char.IsWhiteSpace c ->
            acc
            |> List.fold (fun strAcc ch -> (string ch) + strAcc) ""
            |> Some
        | c -> aux (c :: acc)

    S(fun s -> Some(aux [], s))

(*

    Q: Consider the definition of write There is a reason that the definition
       is S (fun s -> printf "%s" str; Some ((), s)) and not just
       ret (printf "%s" str). For a similar reason, in read, we write
       S (fun s -> Some (aux [], s)) and not ret (aux []).
       What is the problem with using ret in both of these cases?

    A: <Your answer goes here>

    *)

(* Question 4.4 *)

(* You may solve this exercise either using monadic operators or
        using computational expressions. *)

type StateBuilder() =

    member this.Bind(f, x) = bind x f
    member this.Return(x) = ret x
    member this.ReturnFrom(x) = x
    member this.Combine(a, b) = a >>= (fun _ -> b)

let state = new StateBuilder()

let calculateRPN _ = failwith "not implemented"
