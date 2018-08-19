module Webfu.Data.ObjMap
  ( Obj
  , empty
  , allKeys
  , member
  , isSubObj
  , isEmpty
  , insert, (:=)
  , mkObjWithProps
  , Options
  , Option
  , options
  ) where

import Control.Monad.ST
import Data.Array (foldl)
import Data.Function.Uncurried
import Prelude
import Effect (Effect)


-- | `Obj` represents an object on which we can set properties & their values.
foreign import data Obj :: Type

foreign import _copyEff :: forall a b. a -> Effect b

-- | An empty map
foreign import empty :: Obj

-- | Test whether all keys in an `Obj` satisfy a predicate.
foreign import allKeys :: (String -> Boolean) -> Obj -> Boolean

foreign import _lookupKey :: forall a. Fn4 a (String -> a) String Obj a


-- | Insert or replace a key/value pair in a map.
foreign import _insert :: forall a. Fn3 String a Obj Obj
insert :: forall a. String -> a -> Obj -> Obj
insert key val obj = runFn3 _insert key val obj


-- | infix version of `insert`.
infixl 5 insert as :=


-- |
mkObjWithProps :: Array (Obj -> Obj) -> Obj
mkObjWithProps setters = foldl (\obj insertF -> insertF obj) empty setters


-- | Test whether a `String` appears as a key in a map
member :: String -> Obj -> Boolean
member = runFn4 _lookupKey false (const true)


-- | Test whether one obj contains all of the keys contained in another obj
isSubObj :: Obj -> Obj -> Boolean
isSubObj o1 o2 = allKeys (\key1 -> member key1 o2) o1


-- | Test whether a map is empty
isEmpty :: Obj -> Boolean
isEmpty = allKeys (\_ -> false)



type Options = Array (Obj -> Obj)

type Option a = a -> Obj -> Obj

options :: Array (Obj -> Obj) -> Obj
options = mkObjWithProps
