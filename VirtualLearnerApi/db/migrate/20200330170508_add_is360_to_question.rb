class AddIs360ToQuestion < ActiveRecord::Migration[6.0]
  def change
    add_column :questions, :videotype, :string
  end
end
