module Webfu.Data.Cast
( class Cast, cast
) where


class Cast a b where
  cast :: a -> b
