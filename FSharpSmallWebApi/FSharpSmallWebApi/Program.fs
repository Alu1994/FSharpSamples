open Giraffe
open Saturn

let webApp = router {
    pipe_through (setHttpHeader "matthews" "brazil")
    get "/hello" (text "Hello World")
    get "/goodbye" (text "goodbye")
}

let myApp = application {
    use_router webApp
}

run myApp