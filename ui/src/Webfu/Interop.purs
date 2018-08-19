module Webfu.Interop
( JsObject
, toJsObject
, readBoolean
, readInt
, readNumber
, readString
, readChar
, setBoolean
, setBoolean'
, setInt
, setNumber
, setString
, setString'
, setChar
, setFunc
, setEffFunc
, setEventHandler
, runEffMethod0
) where


import Data.Function.Uncurried
import Data.Maybe (Maybe, isJust, fromJust)
import Effect (Effect)
import Prelude (Unit, unit)
import Unsafe.Coerce (unsafeCoerce)


foreign import data JsObject :: Type

foreign import readProp :: forall a. Fn2 String JsObject a
foreign import setPropImpl :: forall a. Fn4 Unit String a JsObject (Effect Unit)
foreign import setPropOrNullImpl :: forall a. Fn6 (Maybe a -> Boolean) (Partial => Maybe a -> a) Unit String (Maybe a) JsObject (Effect Unit)
foreign import setPropEffFuncImpl :: Fn4 Unit String (Unit -> Effect Unit) JsObject (Effect Unit)
foreign import setPropEventHandlerImpl :: forall a. Fn4 Unit String (a -> Effect Unit) JsObject (Effect Unit)



-- | Convert a value into a JsObject.
-- | This function is unsafe, and it is the responsibility of the caller to make
-- | sure the underlying representation for both types are the same.
toJsObject :: forall a. a -> JsObject
toJsObject v = unsafeCoerce v

readBoolean :: String -> JsObject -> Boolean
readBoolean = runFn2 readProp

readInt :: String -> JsObject -> Int
readInt = runFn2 readProp

readNumber :: String -> JsObject -> Number
readNumber = runFn2 readProp

readString :: String -> JsObject -> String
readString = runFn2 readProp

readChar :: String -> JsObject -> Char
readChar = runFn2 readProp


setBoolean :: String -> Boolean -> JsObject -> Effect Unit
setBoolean = runFn4 setPropImpl unit

setBoolean' :: String -> Maybe Boolean -> JsObject -> Effect Unit
setBoolean' = runFn6 setPropOrNullImpl isJust fromJust unit

setInt :: String -> Int -> JsObject -> Effect Unit
setInt = runFn4 setPropImpl unit

setNumber :: String -> Number -> JsObject -> Effect Unit
setNumber = runFn4 setPropImpl unit

setString :: String -> String -> JsObject -> Effect Unit
setString = runFn4 setPropImpl unit

setString' :: String -> Maybe String -> JsObject -> Effect Unit
setString' = runFn6 setPropOrNullImpl isJust fromJust unit

setChar :: String -> Char -> JsObject -> Effect Unit
setChar = runFn4 setPropImpl unit

setFunc :: forall a. String -> a -> JsObject -> Effect Unit
setFunc = runFn4 setPropImpl unit

setEffFunc :: String -> (Unit -> Effect Unit) -> JsObject -> Effect Unit
setEffFunc = runFn4 setPropEffFuncImpl unit

setEventHandler :: forall a. String -> (a -> Effect Unit) -> JsObject -> Effect Unit
setEventHandler = runFn4 setPropEventHandlerImpl unit


foreign import runEffMethod0Impl :: Fn3 Unit String JsObject (Effect Unit)

runEffMethod0 :: String -> JsObject -> Effect Unit
runEffMethod0 = runFn3 runEffMethod0Impl unit
