class QuestionsController < ApplicationController
  before_action :set_question, only: [:show, :update, :destroy]

  # GET /questions
  def index
    if params[:culture]
      c = Culture.find_by(name: params[:culture])
      if c
        @questions = c.questions
      end
    else
      @questions = Question.all
    end

    render json: @questions
  end

  # GET /questions/1
  def show
    render json: @question
  end

  # POST /questions
  def create
    c = Culture.find_by(name: params[:question][:culture])
    q = params[:question]
    @question = Question.new(value: q[:value], answer: q[:answer], text_answer: q[:text_answer], videotype: q[:videotype])
    Culture.find_by(name: q[:culture]).questions << @question
    if @question.save
      render json: @question, status: :created, location: @question    
    # Delete this part?
    else
      render json: @question.errors, status: :unprocessable_entity
    end
  end

  # PATCH/PUT /questions/1
  def update
    if @question.update(question_params)
      render json: @question
    else
      render json: @question.errors, status: :unprocessable_entity
    end
  end

  # DELETE /questions/1
  def destroy
    @question.destroy
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_question
      @question = Question.find(params[:id])
    end

    # Only allow a trusted parameter "white list" through.
    def question_params
      params.require(:question).permit(:value, :answer, :text_answer, :culture)
    end
end
