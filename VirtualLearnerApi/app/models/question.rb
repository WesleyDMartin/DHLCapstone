class Question < ApplicationRecord
    has_and_belongs_to_many :cultures
    validates :value, uniqueness: true
end
